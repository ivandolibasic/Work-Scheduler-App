import { Button } from "react-bootstrap";

export const ButtonComponent = (props) => {
  return (
    <Button variant={props.variant} onClick={props.onClick}>
      {props.body}
    </Button>
  );
};
