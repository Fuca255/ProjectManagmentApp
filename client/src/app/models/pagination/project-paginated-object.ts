import { Project } from "../project/project";

export interface ProjectPaginatedObject {
    projects: Project[],
      maxProjects: number
}
