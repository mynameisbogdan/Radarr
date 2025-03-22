import React, { useCallback, useState } from 'react';
import Form from 'Components/Form/Form';
import FormGroup from 'Components/Form/FormGroup';
import FormInputGroup from 'Components/Form/FormInputGroup';
import FormLabel from 'Components/Form/FormLabel';
import Button from 'Components/Link/Button';
import ModalBody from 'Components/Modal/ModalBody';
import ModalContent from 'Components/Modal/ModalContent';
import ModalFooter from 'Components/Modal/ModalFooter';
import ModalHeader from 'Components/Modal/ModalHeader';
import { inputTypes, kinds, scrollDirections } from 'Helpers/Props';
import translate from 'Utilities/String/translate';
import styles from './SelectEditionModalContent.css';

interface SelectEditionModalContentProps {
  edition: string;
  modalTitle: string;
  onEditionSelect(edition: string): void;
  onModalClose(): void;
}

function SelectEditionModalContent(props: SelectEditionModalContentProps) {
  const { modalTitle, onEditionSelect, onModalClose } = props;
  const [edition, setEdition] = useState(props.edition);

  const onEditionChange = useCallback(
    ({ value }: { value: string }) => {
      setEdition(value);
    },
    [setEdition]
  );

  const onEditionSelectWrapper = useCallback(() => {
    onEditionSelect(edition);
  }, [edition, onEditionSelect]);

  return (
    <ModalContent onModalClose={onModalClose}>
      <ModalHeader>
        {translate('SetEditionModalTitle', { modalTitle })}
      </ModalHeader>

      <ModalBody
        className={styles.modalBody}
        scrollDirection={scrollDirections.NONE}
      >
        <Form>
          <FormGroup>
            <FormLabel>{translate('Edition')}</FormLabel>

            <FormInputGroup
              type={inputTypes.TEXT}
              name="edition"
              value={edition}
              autoFocus={true}
              onChange={onEditionChange}
            />
          </FormGroup>
        </Form>
      </ModalBody>

      <ModalFooter>
        <Button onPress={onModalClose}>{translate('Cancel')}</Button>

        <Button kind={kinds.SUCCESS} onPress={onEditionSelectWrapper}>
          {translate('SetEdition')}
        </Button>
      </ModalFooter>
    </ModalContent>
  );
}

export default SelectEditionModalContent;
