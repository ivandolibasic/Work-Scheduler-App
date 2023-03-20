import { Calendar } from "./Calendar";

const initalTasks = [
  {
    id: 0,
    title: "Team meeting",
    start: new Date().toISOString(),
    end: new Date(new Date().getTime() + 2 * 60 * 60 * 1000).toISOString(),
  },
  {
    id: 2,
    title: "Presentation",
    start: new Date(new Date().getTime() + 4 * 60 * 60 * 1000),
    end: new Date(new Date().getTime() + 5 * 60 * 60 * 1000).toISOString(),
    backgroundColor: "red",
    borderColor: "red",
  },
  {
    id: 3,
    title: "Design meeting",
    start: new Date(new Date().getTime() + 5 * 60 * 60 * 1000),
    end: new Date(new Date().getTime() + 8 * 60 * 60 * 1000).toISOString(),
    backgroundColor: "green",
    borderColor: "green",
  },
]

export const Schedule = () => {
  return (
    <div className="container">
      <Calendar weekendsVisible="true" initialEvents={initalTasks} />
    </div>
  );
};
