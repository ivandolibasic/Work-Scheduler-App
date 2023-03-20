import React from 'react'
import Modal from 'react-bootstrap/Modal'
import Button from 'react-bootstrap/Button'

export default function WorkerUpdateForm({show, close}) {




    return (
      <Modal show={show}>
          <Modal.Header>
            <Modal.Title>Add worker</Modal.Title>
          </Modal.Header>
          <Modal.Body>
              <p>asd</p>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={close}>
              Close
            </Button>
            <Button variant="primary" onClick={close}>
              Add worker
            </Button>
          </Modal.Footer>
      </Modal>
    )
  }
