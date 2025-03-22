import React from 'react';
import Modal from 'Components/Modal/Modal';
import SelectEditionModalContent from './SelectEditionModalContent';

interface SelectEditionModalProps {
  isOpen: boolean;
  edition: string;
  modalTitle: string;
  onEditionSelect(edition: string): void;
  onModalClose(): void;
}

function SelectEditionModal(props: SelectEditionModalProps) {
  const { isOpen, edition, modalTitle, onEditionSelect, onModalClose } = props;

  return (
    <Modal isOpen={isOpen} onModalClose={onModalClose}>
      <SelectEditionModalContent
        edition={edition}
        modalTitle={modalTitle}
        onEditionSelect={onEditionSelect}
        onModalClose={onModalClose}
      />
    </Modal>
  );
}

export default SelectEditionModal;
