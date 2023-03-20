import axios from "axios";



export default axios.create({
  baseURL: "https://localhost:44342/",
  headers: {
    "Content-type": "application/json",
    'Authorization': 'Bearer '+localStorage.getItem('access_token')
  }
});