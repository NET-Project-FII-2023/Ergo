import React, {ChangeEvent} from 'react';
import { Input } from '@material-tailwind/react';
import {InputProps} from "@material-tailwind/react/components/Input";

type ErgoInputProps = {
  onChange?: (value: string) => void,
} & Omit<InputProps, "onChange" | "ref">

const ErgoInput = ({ onChange, ...props }: ErgoInputProps) => {
    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
      onChange && onChange(e.target.value);
    };
  
    return (
      <Input
        size="lg"
        className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
        labelProps={{
          className: "before:content-none after:content-none",
        }}
        onChange={handleChange}
        crossOrigin={undefined}
        {...props}
      />
    );
  };
  

export default ErgoInput;
