import { Button } from '@mui/material'
import type { GetStaticPropsContext, NextPage } from 'next'
import { getSession, GetSessionParams } from 'next-auth/react'
import Head from 'next/head'
import Header from '../components/Header/Header'

const Home: NextPage = props => {
  console.log('home page props')
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
        <Button variant={'contained'} color={'secondary'}>
          Outlined
        </Button>
      </main>
    </div>
  )
}

export default Home

export async function getServerSideProps(ctx: GetStaticPropsContext) {
  return {
    props: {
      session: await getSession(ctx as GetSessionParams)
    }
  }
}
