import { Card } from "react-bootstrap";

export const CardComponent = (props) => {
  return (
    <Card>
      <Card.Img variant="top" src={props.image} />
      <Card.Body>
        <Card.Title>{props.title}</Card.Title>
        <Card.Text>{props.body}</Card.Text>
      </Card.Body>
    </Card>
  );
};
