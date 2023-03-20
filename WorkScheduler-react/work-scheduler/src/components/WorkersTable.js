import React, { useEffect, useState } from 'react'
import Container from 'react-bootstrap/Container';
import http from "../http-common";
import WorkerRow from './WorkerRow';
import Table from 'react-bootstrap/Table';
import { useAuth } from '../context/AuthProvider';

export default function WorkersTable(params) {
  const {accessLevel} = useAuth();
  const [workers,setWorkers] = useState([]);
  const [elements, setElements] = useState();
  const [orderName, setOrderName] = useState('FirstName');
  const [orderDirection, setOrderDirection] = useState('ASC');

  useEffect(()=>{
    
    FetchWorkers();
   
  },[orderName, orderDirection, params.version]);

  useEffect(()=>{
    setElements(MapWorkers());
  },[workers]);

  

  const ChangeOrderName = (name) => {
    if(name!=orderName){
      setOrderName(name);
    }else{
      switch (orderDirection) {
        case 'ASC': setOrderDirection('DESC'); break;
        case 'DESC': setOrderDirection('ASC'); break;
      }
    }
  };

  const FetchWorkers = () => {
    http.get('/api/Worker?OrderName='+orderName+'&OrderDirection='+orderDirection+'&PageSize=10&PageNumber=0').then((response) => {
      setWorkers(response.data);});
  }

  const SelectWorker = (worker) => {
    console.log(worker);
  };

  const MapWorkers = () => (
    <>
    {workers.map((worker) =>
    <WorkerRow worker={worker} onClick={SelectWorker} Delete={Delete} />
    )}
    </>
  )
  const Delete = (worker)=>{
    http.delete('/api/Worker?Id='+worker).then(response => {
      console.log(response);
      FetchWorkers();
    });
  }

  return (
    <Container fluid>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th onClick={e=>ChangeOrderName('FirstName')}>First name</th>
            <th onClick={e=>ChangeOrderName('LastName')}>Last name</th>
            <th onClick={e=>ChangeOrderName('PositionName')}>Work position</th>
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
