import React from 'react'
import { useState } from 'react'
import { useLocation,useNavigate } from 'react-router-dom';
import {
  Input,
  Button,
  Typography,
} from "@material-tailwind/react";
import { toast } from 'react-toastify';
import api from "../../services/api";
import axios from "axios";
const VerifyResetCode = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { email } = location.state || {};
  const [code,setCode] = useState('');
  const handleVerifyCode = async () => {
    if(!code){
      toast.error("Please fill in your code");
    }
    if(!email){
      toast.error("Something went wrong, try again!");
      navigate("/auth/forgot-password");
    }
    console.log(email + " " + code);
    try{
      const response = await api.post("/api/v1/ResetPassword/verify-reset-code", {
        email: email,
        code: code,
      });
      if(response.status === 200){
        toast.success("Code verified successfully");
      }else{
        toast.error(response.data);
      }
    }catch (error) {
      let errorMessage = "Code verification failed";
      if (axios.isAxiosError(error) && error.response) {
        console.error("Server response:", error.response);
        if (error.response.data && error.response.data.validationsErrors) {
          const validationErrors = error.response.data.validationsErrors.join(", ");
          errorMessage += ": " + validationErrors;
        } else if (error.response.data && error.response.data.message) {
          errorMessage += ": " + error.response.data.message;
        }
      } else if (error instanceof Error) {
        errorMessage += ": " + error.message;
      }
      toast.error(errorMessage);
    }
  }
  return (
    <section className="m-8 flex gap-4">
      <div className="w-full lg:w-3/5 mt-24">
        <div className="text-center">
          <Typography variant="h2" className="font-bold mb-4">Reset Your Password</Typography>
          <Typography variant="paragraph" color="blue-gray" className="text-lg font-normal">Enter the confirmation code</Typography>
        </div>
        <div className="mt-8 mb-2 mx-auto w-80 max-w-screen-lg lg:w-1/2">
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium">
              Your code
            </Typography>
            <Input
              size="lg"
              type="text"
              name="email"
              className=" !border-t-blue-gray-200 focus:!border-t-gray-900"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={(e) => setCode(e.target.value)}
            />
          </div>
          <Button className="mt-6" fullWidth onClick={handleVerifyCode}>
            Confirm
          </Button>

        </div>

      </div>
      <div className="w-2/5 h-full hidden lg:block">
        <img
          src="/img/pattern.png"
          className="h-full w-full object-cover rounded-3xl"
        />
      </div>
    </section>
   
  )
}

export default VerifyResetCode