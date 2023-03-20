import React, { useContext } from "react";
import Navbar from "react-bootstrap/Navbar";
import Nav from "react-bootstrap/Nav";
import { useAuth } from "../context/AuthProvider";
import logo from "../assets/logo.png";
export default function NavigationComponent() {
  const { userLogedIn, LogIn, LogOut, username, accessLevel } = useAuth();
  

  return (
    <Navbar
      variant="light"
      bg="light"
      style={{
        flexDirection: "row",
        width: "100%",
        justifyContent: "space-between",
      }}
    >
      <Navbar.Brand href="/">
        <img
          src={logo}
          width="60"
          height="60"
          className="d-inline-block align-top"
          alt="WorkScheduler logo"
        />
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav">
        <Nav style={{ flex: 2 }} className="justify-content-end">
          {userLogedIn ? (
            <>
              <Navbar.Text>Signed in as: {username}</Navbar.Text>
              <Nav.Link href="/schedule">Schedule</Nav.Link>
              {accessLevel == "SuperAdmin" && <>
              
              <Nav.Link href="/Accounts">Accounts</Nav.Link>
              <Nav.Link href="/workers">Workers</Nav.Link>
              <Nav.Link href="/workPositions">Work positions</Nav.Link>
              <Nav.Link href="/tasks">Tasks</Nav.Link></> }
              <Nav.Link
                href="/login"
                onClick={() => {
                  LogOut();
                }}
              >
                Logout
              </Nav.Link>
            </>
          ) : (
            <>
              <Nav.Link href="/login">Login</Nav.Link>
            </>
          )}
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
}
