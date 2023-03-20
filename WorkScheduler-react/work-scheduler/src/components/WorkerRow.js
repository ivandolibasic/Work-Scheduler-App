import React, { useEffect, useState } from 'react'
import { useAuth } from '../context/AuthProvider';
import http from '../http-common'
export default function WorkerRow(props) {
    const [workerId, setWorkerId] = useState('');
    const {accessLevel} = useAuth();
    useEffect(() => {
        setWorkerId(props.worker.Id);
    },[]);

    

  return (
    <tr key={props.worker.Id} >
        <td onClick={e => props.onClick(props.worker)}>{props.worker.FirstName}</td>
        <td onClick={e => props.onClick(props.worker)}>{props.worker.LastName}</td>
        <td onClick={e => props.onClick(props.worker)}>{props.worker.workPosition.PositionName}</td>
        {accessLevel == 'SuperAdmin' && <td style={{color:'red'}} onClick={e => props.Delete(props.worker.Id)}>X</td>}
    </tr>
  )
}
