interface ImportMetaEnv {
  readonly VITE_AWSAccessKey: string;
  readonly VITE_AWSSecretKey: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}