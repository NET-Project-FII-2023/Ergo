import React from 'react';
import { useParams } from 'react-router-dom';

const ProjectDetails = () => {
  const { projectId } = useParams();

  return (
    <div>
      <h2>Project Details</h2>
      <p>Project ID: {projectId}</p>
    </div>
  );
};

export default ProjectDetails;
