import { Button } from '@mui/material'
import type { NextPage } from 'next'
import Head from 'next/head'
import Header from '../components/Header/Header'

const Home: NextPage = () => {
  return (
    <div>
      <Head>
        <title>Godoor</title>
        <meta name="description" content="Godoor" />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main>
        <Header />
        <span className={'underline'}>Hello</span>
        <Button variant={'contained'}>Hello</Button>
        <Button>Doing</Button>
        <Button variant={'outlined'}>Outlined</Button>
      </main>
    </div>
  )
}

export default Home
