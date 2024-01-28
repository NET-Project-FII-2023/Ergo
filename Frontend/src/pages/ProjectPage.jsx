import React from "react";
import { useParams } from "react-router-dom";

const ProjectPage = () => {
  const { projectId } = useParams();

  return (
    <div>
      <h1>Project Details</h1>
      <p>Project ID: {projectId}</p>
      {/* Add more details as needed */}
    </div>
  );
};

export default ProjectPage;
