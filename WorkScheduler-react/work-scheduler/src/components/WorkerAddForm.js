import React, { useEffect, useState } from 'react'
import Modal from 'react-bootstrap/Modal'
import Button from 'react-bootstrap/Button'
import Form from 'react-bootstrap/Form'
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import http from '../http-common'

export default function WorkerAddForm({show, close, availableUsers, workPositions, version, refresh}) {
    const [userId, setUserId] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [position, setPosition] = useState('');



    const AddWorker = () => {
        if(firstName != '' && lastName != '' && position != '') {
            var data = {"Id":userId,
                "FirstName": firstName,
            "LastName": lastName,
            "PositionId": position};
            http.post('/api/Worker', data).then(response => {
                console.log(response.data);
                refresh(version+1);
                handleClose();
            });
        }
    };

    const handleClose = () =>{
        setUserId('');
        setFirstName('');
        setLastName('');
        setPosition('');
        close();
    };

    const optionsUsers = () => (<>
    {
        availableUsers.map((user) =>
        <option value={user.Id}>{user.Username}</option>
        )
    }
    </>)
    const optionsPositions = () => (<>
    {
        workPositions.map((position) => 
        <option value={position.Id}>{position.PositionName}</option>)
    }  </>)
    

  return (
    <Modal show={show}>
        <Modal.Header>
          <Modal.Title>Add worker</Modal.Title>
        </Modal.Header>
        <Modal.Body>
            {availableUsers != null && workPositions != null &&
            <Form>
                <Form.Group>
                    <Form.Select name="users" onChange={(e)=>{setUserId(e.target.value)}}>
                        {
                            optionsUsers()
                        }
                    </Form.Select>
                </Form.Group>
                <Form.Group as={Row} className="mb-3" controlId="formFirstName">
                    <Col>
                        <Form.Label>First name</Form.Label>
                    </Col>
                    <Col>
                    <Form.Control type='text' placeholder='first name' value={firstName} onChange={(e)=>{setFirstName(e.target.value)}}/>
                    </Col>
                </Form.Group>
                <Form.Group as={Row} className="mb-3" controlId="formlastName">
                    <Col>
                        <Form.Label>Last name</Form.Label>
                    </Col>
                    <Col>
                    <Form.Control type='text' placeholder='last name' value={lastName} onChange={(e)=>{setLastName(e.target.value)}}/>
                    </Col>
                </Form.Group>
                <Form.Group as={Row} className="mb-3">
                    <Col>
                    <Form.Label>Position</Form.Label>
                    </Col>
                    <Col>
                    <Form.Select name="positions" onChange={(e)=>{setPosition(e.target.value)}}>
                        {
                            optionsPositions()
                        }
                    </Form.Select>
                    </Col>
                </Form.Group>
            </Form>}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button variant="primary" onClick={AddWorker}>
            Add worker
          </Button>
        </Modal.Footer>
    </Modal>
  )
}
