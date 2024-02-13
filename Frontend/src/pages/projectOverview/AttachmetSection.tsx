import React, {useState, useEffect, useRef} from 'react';
import AttachFileIcon from '@mui/icons-material/AttachFile';
import UploadFileIcon from '@mui/icons-material/UploadFile';
import {Button, Carousel, Dialog} from '@material-tailwind/react';
import api, {s3} from '../../services/api';
import { useUser } from '../../context/LoginRequired';
import {toast} from "react-toastify";

type fileType = {
  file: string;
  fileBlob: Blob;
  fileUrl: string;
  fileId: string;
}

function ImageAttachment({image, onDelete}: { image: fileType, onDelete: () => void}) {
  const [isHovered, setIsHovered] = useState(false)
  const [isImageModalOpen, setIsImageModalOpen] = useState(false)

  return (
    <>
      <div className={"h-full w-full"}
           key={image.fileId}
           onMouseEnter={() => setIsHovered(true)}
           onMouseLeave={() => setIsHovered(false)}
      >
        {isHovered && (
          <div className={"absolute flex gap-5 z-50"}
               style={{left: "50%", top: "50%", transform: "translate(-50%, -50%)"}}
          >
            <a
              href={image.file}
              download={image.fileUrl}
              target={"_blank"}
              rel={"noreferrer"}
            >
              <i className={"fa-regular fa-download text-white text-3xl p-2 cursor-pointer hover:opacity-70"}/>
            </a>
            <i
              className={"fa-regular fa-eye text-blue-400 text-3xl p-2 cursor-pointer hover:opacity-70"}
              onClick={() => setIsImageModalOpen(true)}
            />
            <i
              className={"fa-regular fa-trash text-red-600 text-3xl p-2 cursor-pointer hover:opacity-70"}
              onClick={onDelete}
            />
          </div>
        )}
        <img
          src={image.file}
          alt="file"
          className={`h-full w-full object-cover ${isHovered && "opacity-20"}`}
        />
      </div>
      <Dialog
        open={isImageModalOpen}
        handler={() => setIsImageModalOpen(!isImageModalOpen)}
      >
        <img
          src={image.file}
          alt="file"
          className={`h-full w-full object-cover`}
        />
      </Dialog>
    </>
  )
}

function FileAttachment({file, onDelete}: { file: fileType, onDelete: () => void}) {
  const [isHovered, setIsHovered] = useState(false)

  return (
    <>
      <div
        className={"h-full w-full relative"}
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
      >
        {isHovered && (
          <div className={"absolute flex gap-5 z-50"}
               style={{left: "50%", top: "50%", transform: "translate(-50%, -50%)"}}
          >
            <a
              href={file.file}
              download={file.fileUrl}
              target={"_blank"}
              rel={"noreferrer"}
            >
              <i className={"fa-regular fa-download text-white text-3xl p-2 cursor-pointer hover:opacity-70"}/>
            </a>
            <i
              className={"fa-regular fa-trash text-red-600 text-3xl p-2 cursor-pointer hover:opacity-70"}
              onClick={onDelete}
            />
          </div>
        )}
        <div className={`flex flex-col items-center justify-center w-full h-full bg-surface-darkest ${isHovered && "opacity-10"}`}>
          <i className={"fa-regular fa-file text-4xl font-bold text-white p-5"}/>
          <p className={'text-surface-light'}>File type: {file.fileUrl.split(".")[1].toUpperCase()}</p>
        </div>
      </div>
    </>
  )
}

type AttachmentSectionProps = {
  attachedFiles: {
    fileUrl: string;
    taskFileId: string;
  }[];
  handleFileInputChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

function AttachmentSection({attachedFiles, handleFileInputChange}: AttachmentSectionProps) {
  const currentUser = useUser();
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [files, setFiles] = useState<fileType[]>([]);

  useEffect(() => {
    const fetchFiles = async () => {

      const fetchPromises = attachedFiles.map((file) => {
        return new Promise((resolve, reject) => {
          if (file.fileUrl) {
            s3.getObject({
              Bucket: 'ergo-project',
              Key: file.fileUrl
            }, (err, data) => {
              if (err) {
                console.error("Error", err);
                toast.error("Failed to fetch image");
                reject(err);
              } else if (data.Body) {
                resolve({
                  file: URL.createObjectURL(new Blob([data.Body])),
                  fileBlob: new Blob([data.Body]),
                  fileUrl: file.fileUrl,
                  fileId: file.taskFileId
                });
              }
            });
          } else {
            resolve(null);
          }
        });
      });

      Promise.all(fetchPromises).then((fetchedFiles) => {
        // Filter out any null values that were resolved for files without a URL
        const validFiles = fetchedFiles.filter(file => file !== null);
        setFiles(validFiles as fileType[]);
      }).catch(error => {
        toast("Failed to fetch one or more files")
        console.error("Failed to fetch one or more files", error);
      });
    };

    setFiles([]);
    if (attachedFiles.length) {
      fetchFiles();
    }
  }, [attachedFiles]);


  const onDeleteFile = async (fileId: string) => {
    if (fileId) {
      try {
        const response = await api.delete('/api/v1/Cloud/delete-task-photo', {
          data: {
            photoId: fileId
          },
          headers: {
            Authorization: `Bearer ${currentUser.token}`
          }
        })

        if(response.status === 200) {
          setFiles(files.filter(file => file.fileId !== fileId))
          toast.success("File deleted successfully")
        }
      } catch (err) {
        console.error(err);
        toast.error("Failed to delete file")
      }
    }
  }

  return (
    <div className='flex flex-col mt-4 pr-2'>
      <div className='flex flex-row items-center'>
        <AttachFileIcon className='text-secondary'/>
        <p className='text-gray-300 ml-1 text-md font-semibold'>
          Attachments
        </p>
      </div>
      <input
        type="file"
        className="hidden"
        ref={fileInputRef}
        onChange={handleFileInputChange}
        multiple
      />
      {attachedFiles.length ? (
        <div>
          <Carousel
            className="rounded-lg h-64 w-4/5 bg-surface-darkest mt-2"
            loop={true}
          >
            {files.map((file) => {
              if (/(jpg|jpeg|png|gif|bmp|svg)$/i.test(file.fileUrl)) {
                return <ImageAttachment
                  key={file.fileId}
                  image={file}
                  onDelete={() => onDeleteFile(file.fileId)}
                />
              } else {
                return <FileAttachment
                  key={file.fileId}
                  file={file}
                  onDelete={() => onDeleteFile(file.fileId)}
                />
              }
            })}
            <div className={"flex items-center justify-center w-full h-full bg-surface-darkest"}>
              <i
                className={"fa-regular fa-plus text-4xl font-bold text-white p-5 cursor-pointer hover:opacity-70"}
                onClick={() => fileInputRef.current && fileInputRef.current.click()}
              />
            </div>
          </Carousel>
        </div>
      ) : (
        <div>
          <Button
            className='flex flex-row px-3 bg-surface-darkest py-2 hover:opacity-70 ml-4 mt-1'
            onClick={() => fileInputRef.current && fileInputRef.current.click()}
            size={'sm'}
          >
            <label className="cursor-pointer text-gray-300 flex items-center">
              <p className='font-xs text-surface-light'>Upload</p>
              <UploadFileIcon fontSize='small' className='ml-1'></UploadFileIcon>
            </label>
          </Button>
        </div>
      )}
    </div>
  );
}


export default AttachmentSection;