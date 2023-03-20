import React from 'react'
import { useState } from 'react';
import Container from 'react-bootstrap/esm/Container'
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Button from 'react-bootstrap/Button';
import http from '../http-common';
import { useNavigate } from 'react-router-dom';
import {useAuth} from '../context/AuthProvider';

export default function Login() {
    const [userName, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const {LogIn} = useAuth();
    const navigate = useNavigate();
    const LoginUser = (e) =>{
        e.preventDefault();
        const qs = require('qs');
        const data = {'username': userName, 'password': password, 'grant_type': 'password'};
        
        http.post('/login', qs.stringify({'username': userName, 'password': password, 'grant_type': 'password'}), 
        {headers: {'content-type': 'application/x-www-form-urlencoded'}}).then((res) => {
            localStorage.setItem("access_token", res.data.access_token);
            http.defaults.headers.common['Authorization'] = 'Bearer ' + res.data.access_token;
            
        }).then(()=>{
            http.get('/api/Account?username='+userName+'&password='+password).then((res) => {
                localStorage.setItem("username", res.data.Username);
                localStorage.setItem("accessLevel", res.data.AccessLevel);
                LogIn();
                navigate('/');
            });
            
        });
        
    }
  return (
    <Container className='min-vh-100' fluid>
        <Form className="position-absolute top-50 start-50 translate-middle">   
        <h1>Login</h1> <br/>
            <Form.Group as={Row} className='mb-2' controlId="formUsername">
                <Col md={{span:6} }>
                    <Form.Label>Username</Form.Label>
                </Col>
                <Col md={{span:6}}>
                    <Form.Control type='text' placeholder='enter username' value={userName} onChange={e=>{setUsername(e.target.value)}}/>
                </Col>
            </Form.Group>
            <Form.Group as={Row} className='mb-2' controlId="formPassword">
                <Col md={{span:6} }>
                    <Form.Label>Password</Form.Label>
                </Col>
                <Col md={{span:6}}>
                    <Form.Control type='password' placeholder='enter password' value={password} onChange={e=>{setPassword(e.target.value)}}/>
                </Col>
            </Form.Group><br/>
            <Button type='submit' variant='primary' onClick={LoginUser}>Submit</Button>
        </Form>
    </Container>
  )
}
