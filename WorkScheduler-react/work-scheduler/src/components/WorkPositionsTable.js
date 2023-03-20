import React, { useEffect, useState } from 'react'
import http from '../http-common';
import WorkPositionRow from './WorkPositionRow';
import Container from 'react-bootstrap/Container';
import Table from 'react-bootstrap/Table';


export default function WorkPositionsTable({version, position, setPosition}) {
    const [workPositions,setWorkPositions] = useState([]);
    const [elements, setElements] = useState();
    const [orderName, setOrderName] = useState('PositionName');
    const [orderDirection, setOrderDirection] = useState('ASC');
    

    useEffect(()=>{
        FetchWorkerPositions();
       
      },[orderName, orderDirection,version]);
    
      useEffect(()=>{
        setElements(MapWorkerPositions());
      },[workPositions]);

      

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

      const FetchWorkerPositions = () => {
        http.get('/api/WorkPosition?OrderName='+orderName+'&OrderDirection='+orderDirection+'&PageSize=10&PageNumber=0').then((response) => {
          setWorkPositions(response.data);});
      }
    
      const SelectWorkerPosition = (selectedPosition) => {
          setPosition(selectedPosition);
          console.log("state");
          console.log(position);
          console.log("selected");
          console.log(selectedPosition);
         
      };

      const DeleteWorkerPosition = (position) => {
        http.delete('/api/WorkPosition?Id='+position.Id).then(response => {
          console.log(response);
          FetchWorkerPositions();
        });
      };

      const MapWorkerPositions = () => (
        <>
        {workPositions.map((position) =>
        <WorkPositionRow workPosition={position} onClick={SelectWorkerPosition}  delete={DeleteWorkerPosition}/>
        )}
        </>
      )

  return (
    <Container fluid>
      <Table striped bordered hover>
        <thead>
            <tr>
                <th onClick={e=>ChangeOrderName('PositionName')}>Position name</th>
                <th onClick={e=>ChangeOrderName('Description')}>Description</th>
                <th>Delete</th>
            </tr>
        </thead>
        <tbody>
        {elements}
        </tbody>
      </Table>
    </Container>
  )
}
