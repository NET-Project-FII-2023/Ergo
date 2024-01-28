import {
  Input,
  Checkbox,
  Button,
  Typography,
} from "@material-tailwind/react";
import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import api from "../../services/api";
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const baseURL = "https://localhost:7248/api/v1/Authentication/register";

export function SignUp() {
  const navigate = useNavigate();
  const [inputs, setInputs] = useState({
    username: '',
    name: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const register = async (register_data) => {
    try {
      const response = await api.post("/api/v1/Authentication/register", {
        username: register_data.username,
        name: register_data.name,
        email: register_data.email,
        password: register_data.password,
      });

      if (response.status === 201) {
        navigate('/auth/sign-in');
        toast.success("Registration Successful");
      } else {
        toast.error("Registration failed");
      }
    } catch (error) {
      let errorMessage = "Registration failed";
      if (axios.isAxiosError(error) && error.response) {
        console.error("Server response:", error.response);
        if (error.response.data) {
          errorMessage += ": " + error.response.data;
        }
      } else if (error instanceof Error) {
        errorMessage += ": " + error.message;
      }
      toast.error(errorMessage);
    }

  };

  const validatePassword = (password) => {
    if(password.length < 7){
      toast.error("Password must be at least 7 characters long");
      return false;
    }
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{7,}$/;
    if(!regex.test(password)){
      toast.error("Password must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character");
      return false;
    }
    return true;
  }

  const handleChange = (e) => {
    const {name,value} = e.target;
    setInputs(prev => ({...prev,[name]:value}));
  }

  const handleSignup = (e) => {
    e.preventDefault();

    if(inputs.username === '' || inputs.name === '' || inputs.email === '' || inputs.password === '' || inputs.confirmPassword === ''){
      toast.error("Please fill all the fields");
      return;
    }

    if(inputs.password !== inputs.confirmPassword){
      toast.error("Passwords do not match");
      return;
    }
    if(!validatePassword(inputs.password)){
      return;
    }
      register(inputs);
  }
  return (
    <section className="m-8 flex">
            <div className="w-2/5  h-full hidden lg:block">
        <img
          src="/img/pattern.png"
          className="h-full w-full object-cover rounded-3xl"
        />
      </div>
      <div className="w-full lg:w-3/5 flex flex-col items-center justify-center">
        <div className="text-center">
          <Typography variant="h2" className="font-bold mb-1">Join Us Today</Typography>
          <Typography variant="paragraph" color="blue-gray" className="text-lg font-normal">Enter your email, username, name and password to register.</Typography>
        </div>
        <form className="mt-2 mb-2 mx-auto w-80 max-w-screen-lg lg:w-1/2" onSubmit={handleSignup}>
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium">
              Your email
            </Typography>
            <Input
              size="lg"
              type="email"
              placeholder="name@mail.com"
              name="email"
              className=" !border-t-blue-gray-200 focus:!border-t-gray-900"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={handleChange}
            />
          </div>
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium">
              Your username
            </Typography>
            <Input
              size="lg"
              placeholder="Username"
              name="username"
              className=" !border-t-blue-gray-200 focus:!border-t-gray-900"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={handleChange}
            />
          </div>
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium">
              Your name
            </Typography>
            <Input
              size="lg"
              placeholder="Name"
              name="name"
              className=" !border-t-blue-gray-200 focus:!border-t-gray-900"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={handleChange}
            />
          </div>
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium">
              Your password
            </Typography>
            <Input
              type="password"
              size="lg"
              placeholder="Password"
              name="password"
              className=" !border-t-blue-gray-200 focus:!border-t-gray-900"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={handleChange}
            />
          </div>
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium">
              Confirm password
            </Typography>
            <Input
              type="password"
              size="lg"
              placeholder="Confirm password"
              name="confirmPassword"
              className=" !border-t-blue-gray-200 focus:!border-t-gray-900"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={handleChange}
            />
          </div>
          <Button className="mt-6" fullWidth type="submit">
            Register Now
          </Button>
          <Typography variant="paragraph" className="text-center text-blue-gray-500 font-medium mt-4">
            Already have an account?
            <Link to="/auth/sign-in" className="text-gray-900 ml-1">Sign in</Link>
          </Typography>
        </form>
      </div>
    </section>
  );
}

export default SignUp;
