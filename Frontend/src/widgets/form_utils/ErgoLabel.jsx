import { Typography } from "@material-tailwind/react";

const ErgoLabel = ({labelName}) => {
    return (
        <Typography variant="small" className="mb-1 font-medium text-surface-light">
            {labelName}
        </Typography>
    )
}

export default ErgoLabel;