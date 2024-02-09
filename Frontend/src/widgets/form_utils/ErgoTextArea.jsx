import React from 'react';
import { Textarea } from '@material-tailwind/react';

const ErgoTextarea = ({ placeholder, onChange, value }) => {
    const handleChange = (e) => {
      onChange(e.target.value);
    };
  
    return (
      <Textarea
        placeholder={placeholder}
        className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
        labelProps={{
          className: "before:content-none after:content-none",
        }}
        onChange={handleChange}
        value={value}
        rows={3}
      />
    );
  };
  

export default ErgoTextarea;
