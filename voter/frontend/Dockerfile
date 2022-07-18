# followed: https://docs.docker.com/language/nodejs/build-images/

FROM node:16.15.1
ENV NODE_ENV=production

WORKDIR /app

COPY ["package.json", "package-lock.json", "./"]

RUN npm install --production

COPY . .

CMD ["npm", "start"]