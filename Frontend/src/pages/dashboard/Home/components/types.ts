export type TasksFromAllProjects = {
  [project: string]: TaskItemDto[];
}

export type TaskItemDto = {
  taskItemId: string;
  branchId?: string;
  taskName?: string;
  description?: string;
  deadline: Date;
  createdBy?: string;
  projectId: string;
  assignedUser?: AssignedUserDto;
  state: TaskState;
}

export type AssignedUserDto = {
  userId?: string;
  username?: string;
  name?: string;
}

export enum TaskState {
  ToDo = 1,
  InProgress = 2,
  Done = 3
}

export type TaskStats = {
  count: number;
  footerValue?: string;
}

export type TasksStatsProps = {
  color?: string;
  icon: React.ReactNode;
  title: string;
  count: React.ReactNode;
  footer: React.ReactNode;
}

export type ProjectsStatsItemProps = {
  name: string;
  members: {
    img: string;
    name: string;
    userId: string;
  }[];
  deadline: Date;
  totalTasksCount: number;
  completion: number;
  path: string;
  className: string;
}

export type ProjectsStatsType = {
  [projectId: string]: {
    name: string;
    members: {
      img: string;
      name: string;
      userId: string;
    }[];
    deadline: Date;
    totalTasksCount: number;
    completion: number;
    path: string;
  }
}

export type StatisticsChartProps = {
  chart: object;
  title: React.ReactNode;
  description: React.ReactNode;
}