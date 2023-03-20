import React, { useState } from "react";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";

export const InfoModal = (props) => {
  const [show, setShow] = useState(true);
  const toggle = () => {
    setShow(!show);
  };

  return (
    <div>
      <Modal show={show} onHide={toggle}>
        <Modal.Header closeButton>
          <Modal.Title>{props.title}</Modal.Title>
        </Modal.Header>
        <Modal.Body>{props.body}</Modal.Body>
        <Modal.Footer>
          <Button
            variant="secondary"
            className="btn btn-danger btn-xs"
            onClick={toggle}
          >
            Close
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};
