import React from 'react';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

const ErgoDatePicker = ({ selectedDate, onChange }) => {
  const currentDate = new Date().toLocaleDateString();
  
  return (
    <DatePicker
      selected={selectedDate}
      onChange={onChange}
      wrapperClassName="bg-[#2f2b3a] rounded border"
      className="w-4/5 bg-surface-dark border border-surface-mid-dark rounded p-2 focus:outline-none focus:border-secondary text-surface-light "
      placeholderText={currentDate}
    />
  );
};

export default ErgoDatePicker;
