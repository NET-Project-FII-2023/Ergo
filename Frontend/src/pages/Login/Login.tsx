import React from "react";
import {Box, Button, Container, IconButton, InputAdornment, TextField, Typography} from "@mui/material";

export default function Login() {
    const [username, setUsername] = React.useState('');
    const [password, setPassword] = React.useState('');
    const [showPassword, setShowPassword] = React.useState(false);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        console.log({
            username: username,
            password: password,
        });
        // Here you can add logic to handle the login
    };

    return (
        <Container component="main" maxWidth="xs">
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <Typography component="h1" variant="h5">
                    Sign in
                </Typography>
                <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }}>
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        label="Username"
                        name="username"
                        autoComplete="username"
                        autoFocus
                        onChange={(event) => setUsername(event.target.value)}
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="password"
                        label="Password"
                        autoComplete="current-password"
                        type={showPassword ? 'text' : 'password'}
                        // endAdornment={
                        //     <InputAdornment position="end">
                        //         <IconButton
                        //             aria-label="toggle password visibility"
                        //             onClick={handleClickShowPassword}
                        //             onMouseDown={handleMouseDownPassword}
                        //         >
                        //             {showPassword ? <VisibilityOff /> : <Visibility />}
                        //         </IconButton>
                        //     </InputAdornment>
                        // }
                        onChange={(event) => setPassword(event.target.value)}
                    />
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{ mt: 3, mb: 2 }}
                    >
                        Sign In
                    </Button>
                </Box>
            </Box>
        </Container>
    )
}