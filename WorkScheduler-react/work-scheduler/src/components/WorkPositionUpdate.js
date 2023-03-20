import React, { useEffect } from 'react'
import Card from 'react-bootstrap/Card'
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import http from '../http-common'
export default function WorkPositionUpdate({position, setPosition, version, refresh}) {
  
  const UpdateWorkPosition = (e) => {
    e.preventDefault();
    if(position.PositionName != '' && position.Description != '') {
      http.put('/api/WorkPosition?id='+position.Id, position).then(res=>{
        refresh(version+1);
      })
    }
  }

  return (
        <Form>
            <Form.Group as={Row}>
              <Col>
              <Form.Label>Position name</Form.Label>
              </Col>
              <Col>
                <Form.Control  type='text' value={position.PositionName} onChange={e=>{ setPosition({...position, "PositionName":e.target.value})}} />
              </Col>
            </Form.Group>
            <Form.Group as={Row}>
              <Col>
                <Form.Label>Description</Form.Label>
              </Col>
              <Col>
                <Form.Control type='text' value={position.Description} onChange={e=>{setPosition({...position, "Description":e.target.value})}}/>
              </Col>
            </Form.Group>
            <br/>
            <Row>
            <Button as={Col} type='submit' onClick={UpdateWorkPosition}>Update</Button>
            <Button as={Col} variant='secondary'  onClick={e=>{setPosition(null)}}>Cancel</Button>
            </Row>
        </Form>
  )
}
