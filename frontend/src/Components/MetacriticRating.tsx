import React from 'react';
import { Ratings } from 'Movie/Movie';
import translate from 'Utilities/String/translate';
import styles from './MetacriticRating.css';

interface MetacriticRatingProps {
  ratings: Ratings;
  iconSize?: number;
  hideIcon?: boolean;
}

function MetacriticRating(props: MetacriticRatingProps) {
  const { ratings, iconSize = 14, hideIcon = false } = props;

  const metacriticImage =
    'data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI4OCIgaGVpZ2h0PSI4OCI+CjxjaXJjbGUgZmlsbD0iIzAwMUIzNiIgc3Ryb2tlPSIjRkMwIiBzdHJva2Utd2lkdGg9IjQuNiIgY3g9IjQ0IiBjeT0iNDQiIHI9IjQxLjYiLz4KPHBhdGggdHJhbnNmb3JtPSJ0cmFuc2xhdGUoLTEwLTk2MSkgbWF0cml4KDEuMjc1NjYyOSwtMS4zNDg3NzMzLDEuMzY4NTcxNywxLjI2MzQ5ODcsLTI2Ny4wNDcwNiwxMDY2LjA3NDMpIiBmaWxsPSIjRkZGIgpkPSJtMTI2LjczNDM4LDkyLjA4NzAwMiA1LjA1ODU5LDAgMCwyLjgzMjAzMSBjIDEuODA5ODktMi4yMDA1MDEgMy45NjQ4My0zLjMwMDc2IDYuNDY0ODQtMy4zMDA3ODEgMS4zMjgxMSwyLjFlLTUgMi40ODA0NSwuMjczNDU4IDMuNDU3MDMsLjgyMDMxMiAuOTc2NTUsLjU0Njg5NSAxLjc3NzMzLDEuMzczNzE3IDIuNDAyMzUsMi40ODA0NjkgLjkxMTQ0LTEuMTA2NzUyIDEuODk0NTEtMS45MzM1NzQgMi45NDkyMi0yLjQ4MDQ2OSAxLjA1NDY2LTAuNTQ2ODU0IDIuMTgwOTYtMC44MjAyOTEgMy4zNzg5LTAuODIwMzEyIDEuNTIzNDEsMi4xZS01IDIuODEyNDcsLjMwOTI2NSAzLjg2NzE5LC45Mjc3MzQgMS4wNTQ2NiwuNjE4NTA5IDEuODQyNDIsMS41MjY3MTEgMi4zNjMyOCwyLjcyNDYwOSAuMzc3NTcsLjg4NTQzNCAuNTY2MzcsMi4zMTc3MjQgLjU2NjQxLDQuMjk2ODc1IGwgMCwxMy4yNjE3Mi01LjQ4ODI4LDAgMC0xMS44NTU0NyBjLTNlLTUtMi4wNTcyNzctMC4xODg4My0zLjM4NTQwMS0wLjU2NjQxLTMuOTg0Mzc1LTAuNTA3ODQtMC43ODEyMzMtMS4yODkwOS0xLjE3MTg1OC0yLjM0Mzc1LTEuMTcxODc1LTAuNzY4MjUsMS43ZS01LTEuNDkwOTEsLjIzNDM5Mi0yLjE2Nzk3LC43MDMxMjUtMC42NzcxLC40Njg3NjYtMS4xNjUzOCwxLjE1NTYxNC0xLjQ2NDg0LDIuMDYwNTQ3LTAuMjk5NSwuOTA0OTYxLTAuNDQ5MjQsMi4zMzM5OTgtMC40NDkyMiw0LjI4NzEwOCBsIDAsOS45NjA5NC01LjQ4ODI4LDAgMC0xMS4zNjcxOSBjLTJlLTUtMi4wMTgyMTQtMC4wOTc3LTMuMzIwMjk2LTAuMjkyOTctMy45MDYyNDgtMC4xOTUzMy0wLjU4NTkyMi0wLjQ5ODA2LTEuMDIyMTItMC45MDgyLTEuMzA4NTk0LTAuNDEwMTctMC4yODY0NDItMC45NjY4MS0wLjQyOTY3MS0xLjY2OTkzLTAuNDI5Njg4LTAuODQ2MzYsMS43ZS01LTEuNjA4MDgsLjIyNzg4Mi0yLjI4NTE1LC42ODM1OTQtMC42NzcxLC40NTU3NDUtMS4xNjIxMiwxLjExMzI5Ny0xLjQ1NTA4LDEuOTcyNjU2LTAuMjkyOTgsLjg1OTM4OS0wLjQzOTQ2LDIuMjg1MTctMC40Mzk0NSw0LjI3NzM0IGwgMCwxMC4wNzgxMy01LjQ4ODI4LDB6Ii8+Cjwvc3ZnPg==';

  const value = ratings.metacritic?.value ?? 0;

  return (
    <span className={styles.wrapper}>
      {!hideIcon && (
        <img
          className={styles.image}
          alt={translate('MetacriticRating')}
          src={metacriticImage}
          style={{
            height: `${iconSize}px`,
          }}
        />
      )}
      {value}%
    </span>
  );
}

export default MetacriticRating;
