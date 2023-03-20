import {useState, useEffect} from "react";
import {Table, Button, Form, Stack, Modal} from "react-bootstrap";
import http from "../http-common"

function Tasks() {
  const [tasks, setTasks] = useState([]);
  const [listElements, setListElements] = useState();

  const [description, setDescription] = useState("");
  const [totalHoursNeeded, setTotalHoursNeeded] = useState(0);
  const [status, setStatus] = useState("");

  useEffect(() => {
    setListElements(taskData);
  }, [tasks])

  const fetchTasks = () => {
    http.get("api/Task").then((response) => {
      setTasks(response.data);
    })
  }

  const deleteTask = (id) => {
    http.delete("api/Task?id=" + id).then(() => {
      setTasks(tasks.filter((task) => task.Id !== id));
    })
  }

  const createTask = (e) => {
    e.preventDefault();
    const newTask = {
      Description: description,
      TotalHoursNeeded: totalHoursNeeded,
      Status: status
    };
    http.post("api/Task", newTask).then((response) => {
      setTasks([...tasks, response.data]);
      setDescription("");
      setTotalHoursNeeded("");
      setStatus("");
      fetchTasks();
    })
  }

  const [identifier, setIdentifier] = useState("");
  const [showModal, setShowModal] = useState(false);
  const handleShowModal = (id) => {
    setIdentifier(id);
    setShowModal(true);
  } 
  const handleCloseModal = () => setShowModal(false);

  const updateTask = (identifier) => {
    const newTask = {
      Description: description,
      TotalHoursNeeded: totalHoursNeeded,
      Status: status
    };
    http.put("api/Task?id=" + identifier, newTask).then(() => {
      const updatedTasks = tasks.map(task => {
        if (task.Id === identifier) {
          return {
            ...task,
            Description: description,
            TotalHoursNeeded: totalHoursNeeded,
            Status: status
          };
        } else {
          return task;
        }
      });
      setTasks(updatedTasks);
      fetchTasks();
      handleCloseModal();
    });
    setDescription("");
    setTotalHoursNeeded("");
    setStatus("");
  };

  let taskData = tasks.map(task => {
    return (
      <tr key={task.Id}>
        <td>{task.Description}</td>
        <td>{task.TotalHoursNeeded}</td>
        <td>{task.Status}</td>
        <td>{task.Username}</td>
        <td>{task.DateCreated}</td>
        <td>
          <Stack direction="horizontal" gap={1}>
            <Button variant="warning" onClick={() => handleShowModal(task.Id)}>Update</Button>
            <Button variant="danger" onClick={() => deleteTask(task.Id)}>Delete</Button>
          </Stack>
        </td>
      </tr>
    );
  })

  return (
    <div className="container">
      <br />
      <h1>Tasks</h1>
      <br />
      <Form className="create form" onSubmit={event => event.preventDefault()}>
        <Stack direction="horizontal" gap={1}>
          <Form.Control type="text" placeholder="Enter description" value={description} onChange={e => setDescription(e.target.value)} />
          <Form.Control type="number" placeholder="Enter required time completion" value={totalHoursNeeded} onChange={e => setTotalHoursNeeded(e.target.value)} />
          <Form.Select type="text" placeholder="Enter status" value={status} onChange={e => setStatus(e.target.value)}>
            <option>Select status</option>
            <option value="inProgress">In progress</option>
            <option value="completed">Completed</option>
          </Form.Select>
          <Button type="submit" variant="success" onClick={createTask}>Create</Button>
        </Stack>
      </Form>
      <br />
      <Table striped hover>
        <thead>
          <tr>
            <th>Description</th>
            <th>Time</th>
            <th>Status</th>
            <th>Author</th>
            <th>Date Created</th>
            <th><Button variant="info" onClick={fetchTasks}>Read</Button></th>
          </tr>
        </thead>
        <tbody key="task-table-body">
            {listElements}
        </tbody>
      </Table>
      <div className="modal show" style={{display: "block", position: "initial"}}>
        <Modal show={showModal}>
          <Modal.Header>
            <Modal.Title>Update Task</Modal.Title>
          </Modal.Header>

          <Modal.Body>
            <p>Make changes to selected task</p>
            <Stack gap={1}>
              <Form.Group className="mb-3">
                <Form.Control type="text" placeholder="Enter description" value={description} onChange={e => setDescription(e.target.value)} />
                <Form.Control type="number" placeholder="Enter required time completion" value={totalHoursNeeded} onChange={e => setTotalHoursNeeded(e.target.value)} />
                <Form.Select type="text" placeholder="Enter status" value={status} onChange={e => setStatus(e.target.value)}>
                  <option>Select status</option>
                  <option value="inProgress">In progress</option>
                  <option value="completed">Completed</option>
                </Form.Select>
              </Form.Group>
            </Stack>
          </Modal.Body>

          <Modal.Footer>
            <Stack direction="horizontal" gap={1}>
              <Button variant="primary" onClick={() => updateTask(identifier)}>Save Changes</Button>
              <Button variant="secondary" onClick={handleCloseModal}>Close</Button>
            </Stack>
          </Modal.Footer>
        </Modal>
      </div>
    </div>
  );
}

export default Tasks;