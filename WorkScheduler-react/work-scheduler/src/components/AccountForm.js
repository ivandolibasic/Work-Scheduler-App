import React, { useState } from 'react'
import Col from 'react-bootstrap/Col'
import Row from'react-bootstrap/Row'
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import { useAuth } from "../context/AuthProvider"
import http from '../http-common'

export default function AccountForm({closeForm,refresh,version, accessLevels}) {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [userAccessLevel, setAccessLevel] = useState('');
    const { accessLevel } = useAuth();



    const AddAccount = (e) => {
        if(username != '' && password != '' && userAccessLevel != '') {
            if(accessLevel=='SuperAdmin') {
                http.post('/api/Account', {"Username": username, "Password": password, "AccessLevel": userAccessLevel}).then(res => {
                    console.log(res);
                    refresh(version+1);
                    closeForm();
                });
            }else{
                http.post('api/Account/Admin', {"Username": username, "Password": password, "AccessLevel": userAccessLevel}).then(res => {
                    console.log(res);
                    refresh(version+1);
                    closeForm();
                });
            }
        }
    }



    
    const options = () => {
        if(accessLevel=='SuperAdmin'){
            
            return (<>
                {accessLevels.map(al => (
                    <option value={al.Id}>{al.AccessLevelName}</option>
                ))}
                </>)
        }else{
            var lvl = accessLevels.find(al => al.AccessLevelName == 'User');
           
            return <option value={lvl.Id}>{lvl.AccessLevelName}</option>
        }
        }

  return (
    <Form>
        <Form.Group as={Row} className="mb-3" controlId="formUsername">
            <Col>
            <Form.Label>Username</Form.Label>
            </Col>
            <Col>
            <Form.Control type='text' placeholder='username' value={username} onChange={(e)=>{setUsername(e.target.value)}}/>
            </Col>
        </Form.Group>
        <Form.Group as={Row} className="mb-3" controlId="formPassword">
            <Col>
            <Form.Label>Password</Form.Label>
            </Col>
            <Col>
            <Form.Control type='text' placeholder='password' value={password} onChange={(e)=>{setPassword(e.target.value)}}/>
            </Col>
        </Form.Group>
        <Form.Group as={Row} className="mb-3" controlId="formAccesslevel">
            <Col>
            <Form.Label>AccessLevel</Form.Label>
            </Col>
            <Col>
            <Form.Select name='accessLevel' onClick={(e)=>{setAccessLevel(e.target.value)}}  onChange={(e)=>{setAccessLevel(e.target.value)}}>
                {
                options()
                }
                
            </Form.Select>
            </Col>
        </Form.Group>
        <Row>
        <Button as={Col} type='submit' onClick={AddAccount} >Add position</Button><br/>   
        <Button as={Col} variant='secondary' onClick={closeForm} >Cancel</Button>
        </Row>
    </Form>
  )
}
