import React, { useState } from "react";
import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import listPlugin from "@fullcalendar/list";
import { InfoModal } from "./InfoModal";

export function Calendar(props) {
  const [currentEvents, setCurrentEvents] = useState([]);
  const [modal, setModal] = useState(null);
  
  const handleEventClick = (clickInfo) => {
    if (!modal) {
      setModal(
        <InfoModal
          title={clickInfo.event.title}
          body={clickInfo.event.start.toString()}
        />
      );
    } else setModal(null);
  };

  const handleDateClick = (clickInfo) => {
    if (!modal) {
      setModal(
        <InfoModal
          title={clickInfo.date.toLocaleDateString("en-UK", {
            weekday: "long",
            year: "numeric",
            month: "long",
            day: "numeric",
          })}
          body="You dont have tasks today!"
        />
      );
    } else setModal(null);
  };

  const handleEvents = (event) => {
    setCurrentEvents(event);
  };

  const renderEventContent = (eventInfo) => {
    return (
      <div>
        <b>{eventInfo.timeText}</b>
        <i>{eventInfo.event.title}</i>
      </div>
    );
  };

  return (
    <div>
      <FullCalendar
        weekNumberCalculation="ISO"
        locale="en-UK"
        plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin, listPlugin]}
        headerToolbar={{
          left: "prev,next today",
          center: "title",
          right: "dayGridMonth,timeGridWeek,timeGridDay,listWeek",
        }}
        initialView="dayGridMonth"
        selectable={true}
        selectMirror={true}
        weekends={props.weekendsVisible}
        initialEvents={props.initialEvents}
        eventContent={renderEventContent}
        eventClick={handleEventClick}
        eventsSet={handleEvents}
        dateClick={handleDateClick}
      />
      {modal}
    </div>
  );
}
