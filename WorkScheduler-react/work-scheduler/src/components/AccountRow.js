import React from 'react'
import { useAuth } from "../context/AuthProvider"
export default function AccountRow(props) {
    const { accessLevel } = useAuth();
  const handleClick = (e) =>{
    if(accessLevel == 'SuperAdmin' || (accessLevel == 'Admin' && props.account.AccessLevel == 'User')){
      props.onClick(props.account)
    }

  }

  return (
    <tr key={props.account.Id} >
        <td onClick={e => handleClick(e)}>{props.account.Username}</td>
        <td onClick={e => handleClick(e)}>{props.account.AccessLevel}</td>
        {accessLevel == 'SuperAdmin' && 
        <td style={{color:'red'}} onClick={e=>props.delete(props.account.Id)}>X</td>}
    </tr>
  )
}
