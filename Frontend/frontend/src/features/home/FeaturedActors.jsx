// src/features/home/FeaturedActors.jsx
import React, { useState, useEffect } from 'react'
import useFetch from '../../hooks/useFetch'
import { getAllActors, getActorAverageRating } from '../../api/actors'
import Carousel from '../../components/Carousel'
import ActorCard from '../../components/ActorCard'

export default function FeaturedActors() {
  const { data: raw = [], loading, error } =
    useFetch(() => getAllActors(), [])
  const actors = Array.isArray(raw) ? raw : []

  const sample20 = actors
    .slice()                        
    .sort(() => Math.random() - 0.5)
    .slice(0, 20)

  const [actorsWithScore, setActorsWithScore] = useState([])

  useEffect(() => {
    if (sample20.length === 0) {
      setActorsWithScore([])
      return
    }

    let cancelled = false
    ;(async () => {
      const list = await Promise.all(
        sample20.map(async actor => {
          try {
            const avg = await getActorAverageRating(actor.id)
            return { ...actor, avgRating: avg ?? 0 }
          } catch {
            return { ...actor, avgRating: 0 }
          }
        })
      )
      if (!cancelled) {
        setActorsWithScore(list)
      }
    })()

    return () => { cancelled = true }
  }, [sample20.length]) 

  const top10 = actorsWithScore
    .slice()
    .sort((a, b) => ((b.popularity ?? 0) + (b.avgRating ?? 0)) 
                  - ((a.popularity ?? 0) + (a.avgRating ?? 0)))
    .slice(0, 10)

  const [current, setCurrent] = useState(0)
  const prev = () =>
    setCurrent(i => (i === 0 ? top10.length - 1 : i - 1))
  const next = () =>
    setCurrent(i => (i === top10.length - 1 ? 0 : i + 1))

  if (loading) {
    return (
      <div className="py-6 text-center text-gray-300 dark:text-gray-500">
        Yükleniyor…
      </div>
    )
  }

  if (error) {
    return (
      <div className="py-6 text-center text-danger">
        Hata: {error.message}
      </div>
    )
  }

  if (top10.length === 0) return null

  return (
    <Carousel
      items={top10}
      current={current}
      prev={prev}
      next={next}
      visibleCount={6}
      renderItem={actor => <ActorCard key={actor.id} actor={actor} />}
    />
  )
}
