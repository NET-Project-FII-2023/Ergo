import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios'; // Import Axios library
import {useUser} from "@/context/LoginRequired.jsx";
import api from "@/services/api";

const ProjectDetails = () => {
  const { projectId } = useParams();
  const [taskItems, setTaskItems] = useState([]);
  const {token, userId} = useUser();

  useEffect(() => {
    (async () => {
      if (!token) return;
      if (!userId) return;

      const tasks = await fetchTaskItems(projectId, token);

      if (tasks?.success) {
        setTaskItems(tasks.taskItems);
        console.log("Tasks:")
        console.log(tasks.taskItems);
      } else {
        console.log("Error while fetching user projects");
      }
    })();
  }, []);


  async function getProjectsByUserId(userId, token) {
    try {
      const response = await api.get(`/api/v1/Projects/GetProjectsByUserId/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      if (response.status !== 200) {
        throw new Error(response);
      }
      return response.data;
    } catch (error) {
      console.log(`Error while getting user projects: ${error.response.data}`);
    }
  }

  async function fetchTaskItems (projectId, token) {
    try {
      const response = await api.get(`/api/v1/TaskItems/ByProject/${projectId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status !== 200) {
        throw new Error(response);
      }
      return response.data;
    } catch (error) {
      console.log(`Error while getting tass: ${error.response.data}`);
    }
  }

  return (
    <div>
      <h2>Project Details</h2>
      <p>Project ID: {projectId}</p>
      <div>
        <h3>Task Items:</h3>
        <ul>
          {taskItems.map((taskItem) => (
            <li key={taskItem.taskItemId}>{taskItem.taskName}</li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default ProjectDetails;
