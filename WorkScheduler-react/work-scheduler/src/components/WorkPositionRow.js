import React, { useEffect, useState } from 'react'

export default function WorkPositionRow(props) {
    
  return (
    <tr key={props.workPosition.Id} onClick={e => props.onClick(props.workPosition)}>
        <td>{props.workPosition.PositionName}</td>
        <td>{props.workPosition.Description}</td>
        <td style={{color:'red'}} onClick={e=>props.delete(props.workPosition)}>X</td>
    </tr>
  )
}
