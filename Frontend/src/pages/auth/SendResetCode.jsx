import { useState } from 'react'
import { useNavigate } from "react-router-dom";
import {
  Input,
  Button,
  Typography,
} from "@material-tailwind/react";
import { toast } from 'react-toastify';
import api from "../../services/api";
import axios from "axios";
const SendResetCode = () => {
  const navigate = useNavigate();
  const [email,setEmail] = useState('');
  const handleSendCode = async () => {
    if(!email){
      toast.error("Please fill in your email");
    }else
    {
      try{
        const response = await api.post("/api/v1/ResetPassword/reset-code", {
          email: email,
        });
        if(response.status === 200){
          toast.success("Code sent successfully");
          navigate("/auth/verify-reset", { state: { email: email } });
        }else{
          toast.error(response.data);
        }
      } catch (error) {
        let errorMessage = "Code sending failed";
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
  }
  return (
    <section className="m-8 flex gap-4">
      <div className="w-full lg:w-3/5 mt-24">
        <div className="text-center">
          <Typography variant="h2" className="font-bold mb-4">Reset Your Password</Typography>
          <Typography variant="paragraph" color="blue-gray" className="text-lg font-normal">Enter your email</Typography>
        </div>
        <div className="mt-8 mb-2 mx-auto w-80 max-w-screen-lg lg:w-1/2">
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
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>
          <Button className="mt-6" fullWidth onClick={handleSendCode}>
            Send confirmation code
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

export default SendResetCode