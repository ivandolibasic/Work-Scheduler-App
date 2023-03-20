import logo from "./logo.svg";
import "./App.css";
import Container from "react-bootstrap/Container";
import { Routes, Route } from "react-router-dom";
import NavigationComponent from "./components/NavigationComponent";
import Homepage from "./components/Homepage";
import Login from "./components/Login";
import Tasks from "./components/Tasks";
import WorkersTable from "./components/WorkersTable";
import { Schedule } from "./components/Schedule";
import Workers from "./pages/Workers"
import AuthProvider from './context/AuthProvider';
import WorkPositions from './pages/WorkPositions';
import Accounts from './pages/Accounts';
function App() {
  return (
    <div className="App">
      <AuthProvider>
        <header className="App-header">
          <NavigationComponent />
        </header>
        <main>
          <Container>
            <Routes>
              <Route path="/" element={<Homepage />} />
              <Route path="/login" element={<Login />} />
              <Route path="/tasks" element={<Tasks />} />
              <Route path="/schedule" element={<Schedule />} />
              <Route path="/workers" element={<Workers/>}/>
              <Route path="/workPositions" element={<WorkPositions/>}/>
              <Route path="/accounts"  element={<Accounts/>}/>
              <Route path="/tasks" element={<Tasks />}/>
            </Routes>
          </Container>
        </main>
      </AuthProvider>
    </div>
  );
}

export default App;
