import React, { useRef } from "react";
import { Container, Row, Col } from "react-bootstrap";
import logo from "../assets/logo.png";
import worker from "../assets/worker.jpg";
import superadmin from "../assets/super-admin.jpg";
import admin from "../assets/admin.jpg";
import { CardComponent } from "./Card";
import "../App.css";
import { ButtonComponent } from "./Button";
import { Image } from "./Image";

export default function Homepage() {
  const aboutRef = useRef(null);

  const handleScroll = () => {
    aboutRef.current.scrollIntoView({ behavior: "smooth" });
  };

  return (
    <div>
      <Container style={{ marginBottom: "175px", marginTop: "150px" }}>
        <Row>
          <Col md={7} style={{ marginRight: "150px" }}>
            <h1>Welcome to Work Scheduler</h1>
            <h3>
              App for managing working schedule withing company. Supports
              multiple roles, tasks and requests. Detail view of tasks for every
              day on calendar.
            </h3>
            <ButtonComponent
              variant="primary"
              onClick={handleScroll}
              body="Learn More"
            />
          </Col>
          <Col md={1}>
            <Image src={logo} alt="Logo" className="spin-image" />
          </Col>
        </Row>
      </Container>
      <div ref={aboutRef} className="bg-light py-5">
        <Container>
          <h2>Roles</h2>
          <Row>
            <Col>
              <CardComponent
                image={worker}
                title="Worker"
                body="Workers can request vacation and sick days. 
                      They also have the option
                      to view their tasks on calendar."
              />
            </Col>
            <Col>
              <CardComponent
                image={admin}
                title="Admin"
                body="Admin can approve worker requests. 
                      They can create, update and assign tasks to certain users."
              />
            </Col>
            <Col>
              <CardComponent
                image={superadmin}
                title="Super Admin"
                body="Highest privileges, amonong which is assigning a new admin."
              />
            </Col>
          </Row>
        </Container>
      </div>
    </div>
  );
}
