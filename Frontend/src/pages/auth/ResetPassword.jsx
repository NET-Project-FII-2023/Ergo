import { useEffect, useState } from 'react'
import { useNavigate,useLocation } from "react-router-dom";
import {
  Input,
  Button,
  Typography,
} from "@material-tailwind/react";
import { toast } from 'react-toastify';
import api from "../../services/api";
import axios from "axios";
const ResetPassword = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const {email,code} = location.state || [];
  const validatePassword = (password,confirmPassword) => {
    if(password.length < 7){
      toast.error("Password must be at least 7 characters long");
      return false;
    }
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{7,}$/;
    if(!regex.test(password)){
      toast.error("Password must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character");
      return false;
    }
    if(password !== confirmPassword){
      toast.error("Passwords don't match");
      return false;
    }
    return true;
    
  }
  useEffect(() => {
    if(!email || !code){
      toast.error("Something went wrong, try again!");
      navigate("/auth/send-reset-code");
    }
  }, [])
  const handleClick = async () => {
    if(!password || !confirmPassword){
      toast.error("Please fill in all the fields");
      return;
    }
    if(!validatePassword(password,confirmPassword)){
      return;
    }
    if(!email || !code){
      toast.error("Something went wrong, try again!");
      navigate("/auth/forgot-password");
    }
    try{
      const response = await api.post("/api/v1/Authentication/reset-password", {
        email: email,
        password: password,
        code: code,
      });
      if(response.status === 200){
        toast.success("Password reset successfully");
        navigate("/auth/sign-in");
      }else{
        toast.error(response.data);
      }
    }catch(error){
      let errorMessage = "Password reset failed";
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
      navigate("/auth/send-reset-code")
    }

  }
  return (
    <section className="p-8 flex gap-4 text-surface-light">
      <div className="w-full lg:w-3/5 mt-24">
        <div className="text-center">
          <Typography variant="h2" className="font-bold mb-4">Reset Your Password</Typography>
          <Typography variant="paragraph" color="blue-gray" className="text-lg font-normal text-surface-light-dark">Enter your new password</Typography>
        </div>
        <div className="mt-8 mb-2 mx-auto w-80 max-w-screen-lg lg:w-1/2">
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium text-surface-light">
              Your new password
            </Typography>
            <Input
              size="lg"
              type="password"
              name="password"
              className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
              placeholder='********'
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" color="blue-gray" className="-mb-3 font-medium text-surface-light">
              Cofirm password
            </Typography>
            <Input
              size="lg"
              type="password"
              name="confirm-password"
              className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
              placeholder='********'
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />
          </div>
          <Button className="mt-6 bg-secondary hover:bg-primary" fullWidth onClick={handleClick}>
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

export default ResetPassword