import React, { useEffect, useState } from 'react'
import WorkersTable from '../components/WorkersTable'
import Card from 'react-bootstrap/Card'
import Button from 'react-bootstrap/Button'
import WorkerAddForm from '../components/WorkerAddForm';
import http from '../http-common'


export default function Workers() {


  const [showAddModal, setShowAddModal] = useState(false);
  const [showUpdateModal, setShowUpdateModal] = useState(false);
  const [availableUsers, setAvailableUsers] = useState(null);
  const [version,setVersion] = useState(1);
  const [workPositions, setWorkPositions] = useState(null);

    useEffect(()=>{
      getAvailableUsers();
      getWorkPositions();
    }, [version]);

  


  const getAvailableUsers = () => {
    http.get('/api/UsersWithoutAccount').then(response => {
      setAvailableUsers(response.data);
        console.log(response.data);
      });
  };
  const getWorkPositions = () => {
    http.get('/api/WorkPosition?OrderName=PositionName&OrderDirection=ASC&PageSize=10&PageNumber=0').then(response => {
      setWorkPositions(response.data);
      console.log(response.data);
    });
  };
  
  const closeModal = () => {
    setShowAddModal(false);
    setShowUpdateModal(false);
  };
  return (
    <div>
      <WorkersTable version={version}/>
      <Button onClick={e => {setShowAddModal(true)}}>Add worker</Button>
      {showAddModal == true &&
      <WorkerAddForm show={showAddModal} version={version} refresh={setVersion} close={closeModal} availableUsers={availableUsers} 
      workPositions={workPositions}/>
      }
    </div>
  )
}
