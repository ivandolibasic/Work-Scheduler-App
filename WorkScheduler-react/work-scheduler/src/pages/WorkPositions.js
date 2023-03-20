import React, { useState } from 'react'
import Button from 'react-bootstrap/esm/Button'
import Card from 'react-bootstrap/Card'
import WorkPositionsTable from '../components/WorkPositionsTable'
import WorkPositionForm from '../components/WorkPositionForm';
import WorkPositionUpdate from '../components/WorkPositionUpdate';

export default function WorkPositions() {
    const [formShown, setFormShown] = useState(false);
    const [version,setVersion] = useState(1);
    const [position, setPosition] = useState(null);
    const changeFormShown = () => {
        setFormShown(!formShown);
    };
   
  return (
    <div>
        <WorkPositionsTable version={version} setPosition={setPosition} position={position} />
        { formShown ? <Button onClick={changeFormShown}>Add work position</Button> : 
        <>
        <Card>
            <WorkPositionForm closeForm={changeFormShown} refresh={setVersion} version={version}/>
        </Card>
        <br />
        
        </>}
        {
          position!=null && <Card>
            <WorkPositionUpdate position={position} setPosition={setPosition} refresh={setVersion} version={version}/>
          </Card> 
        }
    </div>
  )
}
