import { defineConfig } from 'orval';

export default defineConfig({
  mywallet: {
    input: 'http://localhost:5155/swagger/v1/swagger.json',
    
    output: {
      mode: 'tags-split',
      target: 'src/api/generated/api.ts', 
      schemas: 'src/api/generated/model', 
      client: 'axios',
      clean: true,
    },
    hooks: {
      afterAllFilesWrite: 'npx prettier --write', 
    },
  },
});