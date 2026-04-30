import PropTypes from 'prop-types';
import React, { Component } from 'react';
import AddNewMovieCollectionMovieModal from 'Collection/AddNewMovieCollectionMovieModal';
import Link from 'Components/Link/Link';
import MonitorToggleButton from 'Components/MonitorToggleButton';
import EditMovieModal from 'Movie/Edit/EditMovieModal';
import MovieIndexProgressBar from 'Movie/Index/ProgressBar/MovieIndexProgressBar';
import MoviePoster from 'Movie/MoviePoster';
import translate from 'Utilities/String/translate';
import styles from './CollectionMovie.css';

class CollectionMovie extends Component {

  //
  // Lifecycle

  constructor(props, context) {
    super(props, context);

    this.state = {
      hasPosterError: false,
      isEditMovieModalOpen: false,
      isNewAddMovieModalOpen: false
    };
  }

  //
  // Listeners

  onEditMoviePress = () => {
    this.setState({ isEditMovieModalOpen: true });
  };

  onEditMovieModalClose = () => {
    this.setState({ isEditMovieModalOpen: false });
  };

  onAddMoviePress = () => {
    this.setState({ isNewAddMovieModalOpen: true });
  };

  onAddMovieModalClose = () => {
    this.setState({ isNewAddMovieModalOpen: false });
  };

  onPosterLoad = () => {
    if (this.state.hasPosterError) {
      this.setState({ hasPosterError: false });
    }
  };

  onPosterLoadError = () => {
    if (!this.state.hasPosterError) {
      this.setState({ hasPosterError: true });
    }
  };

  //
  // Render

  render() {
    const {
      id,
      title,
      status,
      overview,
      year,
      tmdbId,
      images,
      monitored,
      folder,
      statistics = {},
      hasFile,
      isAvailable,
      isExistingMovie,
      isExcluded,
      posterWidth,
      posterHeight,
      detailedProgressBar,
      onMonitorTogglePress,
      collectionId
    } = this.props;

    const {
      movieFileQualities = []
    } = statistics;

    const {
      hasPosterError,
      isEditMovieModalOpen,
      isNewAddMovieModalOpen
    } = this.state;

    const linkProps = id ? { to: `/movie/${tmdbId}` } : { onPress: this.onAddMoviePress };

    const elementStyle = {
      width: `${posterWidth}px`,
      height: `${posterHeight}px`,
      borderRadius: '5px'
    };

    return (
      <div className={styles.content}>
        <div className={styles.posterContainer}>
          {
            isExistingMovie &&
              <div className={styles.editorSelect}>
                <MonitorToggleButton
                  className={styles.monitorToggleButton}
                  monitored={monitored}
                  size={20}
                  onPress={onMonitorTogglePress}
                />
              </div>
          }

          {
            isExcluded ?
              <div
                className={styles.excluded}
                title={translate('Excluded')}
              /> :
              null
          }

          <Link
            className={styles.link}
            style={elementStyle}
            {...linkProps}
          >
            <MoviePoster
              className={styles.poster}
              style={elementStyle}
              images={images}
              size={250}
              lazy={false}
              overflow={true}
              onError={this.onPosterLoadError}
              onLoad={this.onPosterLoad}
            />

            {
              hasPosterError &&
                <div className={styles.overlayTitle}>
                  {title}
                </div>
            }

            <div className={styles.overlayHover}>
              <div className={styles.overlayHoverTitle}>
                {title} {year > 0 ? `(${year})` : ''}
              </div>

              {
                id ?
                  <MovieIndexProgressBar
                    movieId={id}
                    monitored={monitored}
                    status={status}
                    hasFile={hasFile}
                    isAvailable={isAvailable}
                    movieFileQualities={movieFileQualities}
                    width={posterWidth}
                    bottomRadius={true}
                    detailedProgressBar={detailedProgressBar}
                  /> :
                  null
              }
            </div>
          </Link>
        </div>

        <AddNewMovieCollectionMovieModal
          isOpen={isNewAddMovieModalOpen && !isExistingMovie}
          tmdbId={tmdbId}
          title={title}
          year={year}
          overview={overview}
          images={images}
          folder={folder}
          onModalClose={this.onAddMovieModalClose}
          collectionId={collectionId}
        />

        <EditMovieModal
          isOpen={isEditMovieModalOpen}
          movieId={id}
          onModalClose={this.onEditMovieModalClose}
          onDeleteMoviePress={this.onDeleteMoviePress}
        />
      </div>
    );
  }
}

CollectionMovie.propTypes = {
  id: PropTypes.number,
  title: PropTypes.string.isRequired,
  year: PropTypes.number.isRequired,
  status: PropTypes.string.isRequired,
  overview: PropTypes.string,
  monitored: PropTypes.bool,
  collectionId: PropTypes.number.isRequired,
  folder: PropTypes.string,
  statistics: PropTypes.object,
  hasFile: PropTypes.bool,
  isAvailable: PropTypes.bool,
  images: PropTypes.arrayOf(PropTypes.object).isRequired,
  posterWidth: PropTypes.number.isRequired,
  posterHeight: PropTypes.number.isRequired,
  detailedProgressBar: PropTypes.bool.isRequired,
  isExistingMovie: PropTypes.bool,
  isExcluded: PropTypes.bool,
  tmdbId: PropTypes.number.isRequired,
  imdbId: PropTypes.string,
  youTubeTrailerId: PropTypes.string,
  onMonitorTogglePress: PropTypes.func.isRequired
};

CollectionMovie.defaultProps = {
  hasFile: false
};

export default CollectionMovie;
