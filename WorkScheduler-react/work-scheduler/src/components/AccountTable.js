import React, { useEffect, useState } from 'react'
import Container from 'react-bootstrap/Container';
import Table from 'react-bootstrap/Table';
import http from '../http-common'
import AccountRow from './AccountRow';
import { useAuth } from "../context/AuthProvider"
export default function ({version, updateAccount}) {
    const [accounts,setAccounts] = useState([]);
    const [elements, setElements] = useState();
    const { accessLevel } = useAuth();
    

    useEffect(()=>{
        FetchAccounts();
    },[version]);

    useEffect(()=>{
        setElements(MapAccounts());
      },[accounts]);


      const FetchAccounts = () => {
        http.get('/api/Account').then((response) => {
          setAccounts(response.data);});
      }


      const SelectAccount = (account) => {
        updateAccount(account);
      };

      const MapAccounts= () => (<>
      {accounts.map((account)=>
      <AccountRow account={account} onClick={SelectAccount} delete={DeleteAccount}/>)}
      </>)

      const DeleteAccount = (account) => {
        http.delete('/api/Account?Id='+account).then((response) => {
            console.log(response.data);
            FetchAccounts();
        });
      }


  return (
    <Container fluid>
      <Table striped bordered hover>
        <thead>
            <tr>
                <th>Username</th>
                <th>Access level</th>
                {accessLevel == 'SuperAdmin' && <th>Delete</th>}
            </tr>
        </thead>
        <tbody>
        {elements}
        </tbody>
      </Table>
    </Container>
  )
}
