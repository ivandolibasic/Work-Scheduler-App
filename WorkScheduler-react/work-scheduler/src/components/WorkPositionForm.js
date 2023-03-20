import React, { useState } from 'react'
import Button from 'react-bootstrap/esm/Button';
import Col from 'react-bootstrap/esm/Col';
import Row from 'react-bootstrap/esm/Row';
import Form from 'react-bootstrap/Form'
import http from '../http-common'

export default function WorkPositionForm({closeForm, version, refresh}) {
    const [positionName, setPositionName] = useState('');
    const [description, setDescription] = useState('');


    const AddWorkPosition = (e) => {
        if(positionName != '' && description != '') {
            http.post('/api/WorkPosition',{"PositionName": positionName, "Description": description}).then(res => {
                console.log(res);
                refresh(version+1);
                closeForm();
            });
        }
    }
    
  return (
    <Form>
        <Form.Group as={Row} className="mb-3" controlId="formPositionName">
            <Col>
            <Form.Label>Position name</Form.Label>
            </Col>
            <Col>
            <Form.Control type='text' placeholder='position' value={positionName} onChange={(e)=>{setPositionName(e.target.value)}}/>
            </Col>
        </Form.Group>
        <Form.Group as={Row} className="mb-3" controlId="formDescription">
            <Col>
            <Form.Label>Description</Form.Label>
            </Col>
            <Col>
            <Form.Control type='text' placeholder='description' value={description} onChange={(e)=>{setDescription(e.target.value)}}/>
            </Col>
        </Form.Group>
        <Row>
        <Button as={Col} type='submit' onClick={AddWorkPosition} >Add position</Button><br/>   
        <Button as={Col} variant='secondary' onClick={closeForm} >Cancel</Button>
        </Row>
    </Form>
  )
}
