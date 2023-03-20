import React, { useEffect, useRef, useState } from 'react'
import AccountTable from '../components/AccountTable'
import Button from 'react-bootstrap/esm/Button';
import Card from 'react-bootstrap/Card'
import AccountForm from '../components/AccountForm';
import http from '../http-common'
import AccountUpdate from '../components/AccountUpdate';
import Modal from 'react-bootstrap/Modal'

export default function Accounts() {
    const [formShown, setFormShown] = useState(true);
    const [version,setVersion] = useState(1);
    const [accessLevels, setAccessLevels] = useState(null);
    const [show,setShow] = useState(false);
    const [updateAccount, setUpdateAccount] = useState(null);
    const dataFetchedRef = useRef(false);
    
    const changeFormShown = () => {
        setFormShown(!formShown);
    };
    
    useEffect(()=>{
        if(dataFetchedRef.current == true) return;
        getAccessLevels();
        dataFetchedRef.current = true;
    },[]);

   

    const getAccessLevels = () =>{
        http.get('/api/AccessLevel').then(response =>{      
            setAccessLevels(response.data);
        });
    }

    const UpdateAccount = (account)=>{
        
        setUpdateAccount(account);
        setShow(true);
    }
    const CloseModal = () =>{setShow(false);}


  return (
    <div>
        <AccountTable version={version} updateAccount={UpdateAccount}/>
        { formShown ? <Button onClick={changeFormShown}>Add user</Button> : 
        <>
        <Card>
            {
            accessLevels != null && <AccountForm accessLevels={accessLevels} closeForm={changeFormShown} refresh={setVersion} version={version}/>
            }
        </Card>
        <br />
        
        </>}
        {updateAccount && <AccountUpdate show={show} close={CloseModal} 
        updateAccount={updateAccount} setUpdateAccount={setUpdateAccount} accessLevels={accessLevels} refresh={setVersion} version={version}/>}
        
       
    </div>
  )
}
