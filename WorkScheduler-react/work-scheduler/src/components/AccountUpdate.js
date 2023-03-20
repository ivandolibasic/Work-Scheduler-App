import React, { useState } from 'react'
import Modal from 'react-bootstrap/Modal'
import Button from 'react-bootstrap/Button'
import Form from 'react-bootstrap/Form'
import Col from 'react-bootstrap/Col'
import Row from 'react-bootstrap/Row'
import http from '../http-common'


import { useAuth } from "../context/AuthProvider"

export default function AccountUpdate({show, close, updateAccount, setUpdateAccount, accessLevels, refresh, version}) {
    const [password, setPassword] = useState('');
    const [userAccessLevel, setAccessLevel] = useState('');
    const { accessLevel } = useAuth();
    const handleClose = ()=>{
        setPassword('');
        setAccessLevel('');
        close();
    };
    
    const Update = () => {
      
      if(updateAccount.Username != '') {
        var data = {"Username": updateAccount.Username, "AccessLevel": userAccessLevel}
        if(password != ''){
          data = {...data, "Password": password}
        }
        
        if(accessLevel=='SuperAdmin') {
            http.put('/api/Account?Id='+updateAccount.Id, data).then(res => {
                console.log(res);
                refresh(version+1);
                handleClose();
            });
        }else{
            http.put('api/Account/Admin?Id='+updateAccount.Id, data).then(res => {
                console.log(res);
                refresh(version+1);
                handleClose();
            });
        }
      }
    }

    const options = () => {
      if(accessLevel=='SuperAdmin'){
          
          return (<>
              {accessLevels.map(al => (
                updateAccount.AccessLevel == al.AccessLevelName ?
                  <option value={al.Id} selected>{al.AccessLevelName}</option> :
                  <option value={al.Id}  >{al.AccessLevelName}</option>
              ))}
              </>)
      }
      }

  return (
    <Modal show={show}>
        <Modal.Header>
          <Modal.Title>Update account</Modal.Title>
        </Modal.Header>
        <Modal.Body>
            <Form>
                <Form.Group as={Row} className="mb-3" controlId="formUsername">
                    <Col>
                    <Form.Label>Username</Form.Label>
                    </Col>
                    <Col>
                    <Form.Control type='text'  value={updateAccount.Username} onChange={e => setUpdateAccount({...updateAccount, "Username": e.target.value})}/>
                    </Col>
                </Form.Group>
                <Form.Group as={Row} className="mb-3" controlId="formPassword">
                    <Col>
                    <Form.Label>Password</Form.Label>
                    </Col>
                    <Col>
                    <Form.Control type='text' placeholder='new password'  value={password} onChange={(e)=>{setPassword(e.target.value)}}/>
                    </Col>
                </Form.Group>
                {
                  accessLevel == 'SuperAdmin' &&
                  <Form.Group as={Row} className="mb-3" controlId="formAccesslevel">
                    <Col>
                    <Form.Label>AccessLevel</Form.Label>
                    </Col>
                    <Col>
                    <Form.Control as="select" name='accessLevel' onClick={(e)=>{setAccessLevel(e.target.value)}}  
                    onChange={(e)=>{setAccessLevel(e.target.value)}}>   
                    {
                      options()
                    }    
                    </Form.Control>
                    </Col>
                </Form.Group>
                  }
            </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button variant="primary" onClick={Update}>
            Save Changes
          </Button>
        </Modal.Footer>
    </Modal>
  )
}
