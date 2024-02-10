import axios from 'axios';
import AWS from "aws-sdk";

AWS.config.update({
  region: 'eu-central-1',
  credentials: {
    accessKeyId: import.meta.env.VITE_AWSAccessKey,
    secretAccessKey: import.meta.env.VITE_AWSSecretKey,
  }
});

export const s3 = new AWS.S3({
  params: { Bucket: 'ergo-project' },
  region: 'eu-central-1',
});

const instance = axios.create({
  baseURL: 'https://localhost:7248/',
  headers: {
    'Content-Type': 'application/json',
  },
});

export default instance;