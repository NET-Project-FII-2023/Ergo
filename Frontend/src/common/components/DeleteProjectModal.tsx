import { Modal } from "@mui/material";
import ErgoInput from "../../widgets/form_utils/ErgoInput";
import { useState } from "react";
import { Button } from "@material-tailwind/react";

export function DeleteProjectModal({open, onClose, onConfirm, projectData} : DeleteProjectModalProps) {
  const [confirmationText, setConfirmationText] = useState("");
  const [isValid, setIsValid] = useState(false);

  const handleChange = (value: string) : void => {
    setConfirmationText(value);
    if (value === "CONFIRM") {
      setIsValid(true);
    }
  };

  const handleSubmit = () : void => {
    setConfirmationText("");
    setIsValid(false);
    onConfirm(projectData.id);
    onClose();
  };

  return (
    <Modal open={open} onClose={() => {setConfirmationText(""); onClose();}}>
      <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 sm:px-6 sm:min-w-96 sm:max-w-[500px] w-[90vw] text-surface-light bg-[#2f2b3a] shadow-lg p-4 rounded-md">
        <h3 className="font-bold text-xl mb-4">Delete project <span className="text-primary">{projectData.name}</span> ?</h3>
        <p>You are about to delete your project.</p>
        <p>
          <span className="text-white font-bold">Note:</span>
          &nbsp;This action is&nbsp;
          <span className="text-red-400 font-bold">permanent</span>
          &nbsp;and&nbsp;
          <span className="text-red-400 font-bold">irreversible</span>. 
        </p>
        <hr className="my-4" />
        <p className="mb-2">Type <span className="font-bold text-white">CONFIRM</span> to proceed with deletion:</p>
        <ErgoInput
          value={confirmationText}
          onChange={handleChange}
        />
        <div className="flex justify-end mt-4">
        <Button size="sm" onClick={() => {setConfirmationText(""); setIsValid(false); onClose();}} className="mr-2 bg-surface-mid uppercase">
          Cancel
        </Button>
        <Button size="sm" disabled={!isValid} onClick={handleSubmit} className="bg-red-500 uppercase">
          Delete
        </Button>
        </div>
      </div>
    </Modal>
  )
}

type DeleteProjectModalProps = {
  open: boolean;
  onClose: () => void;
  onConfirm: (projectId : string) => void;
  projectData: {
    id: string;
    name: string;
  }
}

export default DeleteProjectModal;