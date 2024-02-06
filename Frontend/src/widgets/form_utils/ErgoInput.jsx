import React from 'react';
import { Input } from '@material-tailwind/react';

const ErgoInput = ({ placeholder, onChange, value }) => {
    const handleChange = (e) => {
      onChange(e.target.value);
    };
  
    return (
      <Input
        size="lg"
        placeholder={placeholder}
        className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
        labelProps={{
          className: "before:content-none after:content-none",
        }}
        onChange={handleChange}
        value={value}
      />
    );
  };
  

export default ErgoInput;
