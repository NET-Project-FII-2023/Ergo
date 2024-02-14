import {
  Input,
  Button,
  Typography,
} from "@material-tailwind/react";
import {Link, useNavigate} from "react-router-dom";
import {useEffect, useState} from "react";
import {toast} from "react-toastify";
import api from "@/services/api";
import axios from "axios";


export function SignIn() {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  useEffect(() => {
    localStorage.removeItem("token");
  }, []);

  const handleSignIn = async () => {
    if(!username || !password) {
      toast.error("Please fill all fields");
    }

    try {
      const response = await api.post("/api/v1/Authentication/login", {
        username: username,
        password: password,
      });

      if(response.status === 200) {
        localStorage.setItem("token", response.data);
        toast.success("Login Successful");
        navigate('/');
      }
    } catch (error) {
      let errorMessage = "Login failed";
      if (axios.isAxiosError(error) && error.response) {
        if (error.response.data) {
          if(error.response.data.validationsErrors)
            errorMessage += ": " + error.response.data.validationsErrors[0];
          else
            errorMessage += ": " + error.response.data;
        }
      } else if (error instanceof Error) {
        errorMessage += ": " + error.message;
      }

      toast.error(errorMessage);
    }
  }

  return (
    <section className="p-8 bg-surface-darkest flex gap-4 text-surface-light">
      <div className="w-full lg:w-3/5 mt-24">
        <div className="text-center">
          <Typography variant="h2" className="font-bold mb-4">Sign In</Typography>
          <Typography variant="paragraph" className="text-lg font-normal text-surface-light-dark">Enter your email and password to Sign In.</Typography>
        </div>
        <form className="mt-8 mb-2 mx-auto w-80 max-w-screen-lg lg:w-1/2">
          <div className="mb-1 flex flex-col gap-6">
            <Typography variant="small" className="-mb-3 font-medium text-surface-light">
              Your username
            </Typography>
            <Input
              size="lg"
              placeholder="Username"
              className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={(e) => setUsername(e.target.value)}
            />
            <Typography variant="small" className="-mb-3 font-medium text-surface-light">
              Password
            </Typography>
            <Input
              type="password"
              size="lg"
              placeholder="********"
              className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <Button className="mt-6 bg-secondary hover:bg-primary" fullWidth onClick={handleSignIn}>
            Sign In
          </Button>
          <Typography variant="small" className="text-center text-surface-light-dark font-medium mt-4">
            Not registered?
            <Link to="/auth/sign-up" className="text-secondary ml-1 hover:text-primary">Create account</Link>
          </Typography>
          <Typography variant="small" className="text-center text-surface-light-dark font-medium mt-4">
            Forgot password?
            <Link to="/auth/send-reset-code" className="text-secondary ml-1 hover:text-primary">Reset password</Link>
          </Typography>
        </form>

      </div>
      <div className="w-2/5 h-full hidden lg:block">
        <img
          src="/img/pattern.png"
          className="h-full w-full object-cover rounded-3xl"
        />
      </div>

    </section>
  );
}

export default SignIn;
